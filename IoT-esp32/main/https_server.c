#include <esp_log.h>
#include <esp_system.h>
#include <esp_https_server.h>
#include "esp_tls.h"
#include <stdio.h>
#include <stdlib.h>

#include "global.h"
#include "StationStates.h"
#include "https_client.h"

#define LOGIN_URI "/login/"
#define BACKEND_URL_URI "/backend_url/"
#define STATION_ID_URI "/station_id/"
#define START_CHARGING_URI ESP32_API"/start_charging/"

#define MAXCHAR 1024
#define USERS_FILE_NAME MOUNT_POINT"/user.csv"

#define LOGIN_DATA_MAX_LENGTH 100
#define BACKEND_URL_MAX_LENGTH 50
#define STATION_ID_MAX_LENGTH 4
#define REQ_ENERGY_MAX_LENGTH 6

#define USERS_MAX_NUM 3
#define USER_ID_MAX_SIZE 10
#define COOKIE_NAME "user_id"

static int users_num = 0;
static int users_id[USERS_MAX_NUM];

static const char *TAG = "https_server";

static char* get_cookie_val(httpd_req_t *req, char *cookie_name)
{
    size_t hdr_len_cookie = httpd_req_get_hdr_value_len(req, "Cookie");
    char *cookie_str = NULL;
    char *cookie_val = NULL;

    if (hdr_len_cookie > 0) {
        cookie_str = malloc(hdr_len_cookie + 1);
        if (cookie_str == NULL) {
            ESP_LOGE(TAG, "Failed to allocate memory for cookie string");
            return NULL;
        }

        if (httpd_req_get_hdr_value_str(req, "Cookie", cookie_str, hdr_len_cookie + 1) != ESP_OK) {
            ESP_LOGW(TAG, "Cookie not found in header uri:[%s]", req->uri);
            free(cookie_str);
            return NULL;
        }

        cookie_val = (char*)malloc(sizeof(char) * USER_ID_MAX_SIZE);
        strcpy(cookie_val, cookie_str + strlen(cookie_name) + 1);
        free(cookie_str);
    }
    return cookie_val;
}

static bool user_authorised(httpd_req_t *req)
{
    char *cookie_val = get_cookie_val(req, COOKIE_NAME);
    if (cookie_val != NULL) {
        int user_id = strtol(cookie_val, NULL, 10);
        for (int i = 0; i < users_num; ++i) {
            if (users_id[i] == user_id)
            {
                return true;
            }
        }
    }
    return false;
}

static esp_err_t login_get_handler(httpd_req_t *req)
{
    ESP_LOGI(TAG, "[login_get_handler]");
    extern const unsigned char upload_login_script_start[] asm("_binary_upload_login_script_html_start");
    extern const unsigned char upload_login_script_end[]   asm("_binary_upload_login_script_html_end");
    const size_t upload_login_script_size = (upload_login_script_end - upload_login_script_start);

    httpd_resp_send_chunk(req, (const char *)upload_login_script_start, upload_login_script_size);
    httpd_resp_sendstr_chunk(req, NULL);

    return ESP_OK;
}

static void create_default_user(char *file_name)
{
    FILE *f = fopen(file_name, "w");
    if (f == NULL) {
        ESP_LOGE(TAG, "Failed to open %s for writing", file_name);
        return;
    }
    fprintf(f, "username;password_hash\n");
    fprintf(f, "%s;%s\n", "admin", "21232f297a57a5a743894a0e4a801fc3");
    fclose(f);
}

static bool user_exists(char *file_name, char *user_data)
{
    FILE *fp = fopen(file_name, "r");
    if (fp == NULL) {
        ESP_LOGE(TAG, "Failed to open %s for reading", file_name);
        create_default_user(file_name);
        return false;
    }

    int row = 0;
    int column = 0;
    char buffer[MAXCHAR];

    char *username = NULL;
    char *password_hash = NULL;
    char *user_info = NULL;

    while (fgets(buffer, MAXCHAR, fp))
    {
        column = 0;
        ++row;
        if (row == 1) {
            continue;
        }

        char *value = strtok(buffer, ";");
        while (value) {
            if (column == 0) {
                username = value;
            }

            if (column == 1) {
                password_hash = value;
            }

            value = strtok(NULL, ";");
            ++column;
        }

        asprintf(&user_info, "%s:%s", username, password_hash);
        if (!strncmp(user_info, user_data, strlen(user_data))) {
            fclose(fp);
            return true;
        }
    }

    fclose(fp);
    return false;
}

static esp_err_t login_post_handler(httpd_req_t *req)
{
    ESP_LOGI(TAG, "[login_post_handler]");
    if (users_num == USERS_MAX_NUM) {
        ESP_LOGE(TAG, "Number of users has reached max value - %d", USERS_MAX_NUM);
        return ESP_FAIL;
    }

    char user_data[LOGIN_DATA_MAX_LENGTH];
    strcpy(user_data, req->uri + strlen(LOGIN_URI));

    if (user_exists(USERS_FILE_NAME, user_data)) {
        int r = rand();
        users_id[users_num++] = r;

        char resp[USER_ID_MAX_SIZE];
        sprintf(resp, "%d", r);

        return httpd_resp_send(req, resp, HTTPD_RESP_USE_STRLEN);
    } else {
        return httpd_resp_send_err(req, HTTPD_401_UNAUTHORIZED, "User with such login and password not found!");
    }
}

static void remove_user(httpd_req_t *req)
{
    char *cookie_val = get_cookie_val(req, COOKIE_NAME);
    if (cookie_val == NULL) {
        return;
    }

    int user_id = strtol(cookie_val, NULL, 10);
    for (int i = 0; i < users_num; ++i) {
        if (users_id[i] == user_id)
        {
            users_id[--users_num] = 0;
            return;
        }
    }
}

static esp_err_t logout_post_handler(httpd_req_t *req)
{
    ESP_LOGI(TAG, "[logout_post_handler]");
    remove_user(req);
    return login_get_handler(req);
}

static esp_err_t root_get_handler(httpd_req_t *req)
{
    ESP_LOGI(TAG, "[root_get_handler]");
    if (!user_authorised(req)) {
        return login_get_handler(req);
    }

    extern const unsigned char upload_script_start[] asm("_binary_upload_script_html_start");
    extern const unsigned char upload_script_end[]   asm("_binary_upload_script_html_end");
    const size_t upload_script_size = (upload_script_end - upload_script_start);

    httpd_resp_send_chunk(req, (const char *)upload_script_start, upload_script_size);
    httpd_resp_sendstr_chunk(req, NULL);

    return ESP_OK;
}

// static esp_err_t favicon_get_handler(httpd_req_t *req)
// {
//     ESP_LOGI(TAG, "[favicon_get_handler]");
//     extern const unsigned char favicon_ico_start[] asm("_binary_favicon_ico_start");
//     extern const unsigned char favicon_ico_end[]   asm("_binary_favicon_ico_end");
//     const size_t favicon_ico_size = (favicon_ico_end - favicon_ico_start);
//     httpd_resp_set_type(req, "image/x-icon");
//     httpd_resp_send(req, (const char *)favicon_ico_start, favicon_ico_size);
//     return ESP_OK;
// }

static esp_err_t rewrite_file(char* file_name, char* value)
{
    ESP_LOGI(TAG, "Rewriting value to the %s...", file_name);
    FILE *f = fopen(file_name, "w");
    if (f == NULL) {
        ESP_LOGE(TAG, "Failed to open %s for writing", file_name);
        return ESP_FAIL;
    }

    fprintf(f, "%s", value);
    fclose(f);
    return ESP_OK;
}

static esp_err_t backend_url_post_handler(httpd_req_t *req)
{
    ESP_LOGI(TAG, "[backend_url_post_handler]");
    char backend_url[BACKEND_URL_MAX_LENGTH];
    strcpy(backend_url, req->uri + strlen(BACKEND_URL_URI));
    SystemBackendUrl = backend_url;
    
    if (ESP_FAIL == rewrite_file(BACKEND_URL_FILE_NAME, SystemBackendUrl)) {
        return httpd_resp_send_err(req, HTTPD_500_INTERNAL_SERVER_ERROR,
            "Backend URL can't be saved to the local storage");
    }
    return httpd_resp_sendstr(req, "Backend url was successfully saved!");
}

static esp_err_t station_id_post_handler(httpd_req_t *req)
{
    ESP_LOGI(TAG, "[station_id_post_handler]");
    char station_id[STATION_ID_MAX_LENGTH];
    strcpy(station_id, req->uri + strlen(STATION_ID_URI));
    StationId = station_id;
    
    if (ESP_FAIL == rewrite_file(STATION_ID_FILE_NAME, StationId)) {
        return httpd_resp_send_err(req, HTTPD_500_INTERNAL_SERVER_ERROR,
            "Station id can't be saved to the local storage");
    }
    return httpd_resp_sendstr(req, "Station id was successfully saved!");
}

static char* get_datetime_now()
{
    time_t now;
    struct tm timeinfo;

    size_t size = 64;
    char *buffer = (char*)malloc(sizeof(char) * size);

    time(&now);
    setenv("TZ", "EET-2EEST,M3.5.0/3,M10.5.0/4", 1);
    tzset();
    localtime_r(&now, &timeinfo);
    strftime(buffer, size, "%d.%m.%Y %H:%M:%S", &timeinfo);
    
    return buffer;
}

static esp_err_t start_charging_post_handler(httpd_req_t *req)
{
    ESP_LOGI(TAG, "[start_charging_post_handler]");
    ESP_LOGI(TAG, "Station state - %s", station_state_to_str(StationState));

    if (StationState != READY_STATE) {
        return httpd_resp_send_err(req, HTTPD_400_BAD_REQUEST,
            "Station isn't ready for charging!");
    }

    StationState = CHARGING_STATE;
    https_client_post_state();

    Transaction = calloc(3, sizeof(struct transaction));
    if (!Transaction) {
        ESP_LOGE(TAG, "Failed to allocate memory for transaction");
        return ESP_FAIL;
    }

    Transaction->start_datetime = get_datetime_now();
    ESP_LOGI(TAG, "Charging start datetime - %s", Transaction->start_datetime);

    char requested_energy[REQ_ENERGY_MAX_LENGTH];
    strcpy(requested_energy, req->uri + strlen(START_CHARGING_URI));
    ESP_LOGI(TAG, "Requested energy - %s", requested_energy);

    // send cmd to stm32 with requested energy

    return httpd_resp_sendstr(req, "Request was successfully processed!");
}

static esp_err_t stop_charging_get_handler(httpd_req_t *req)
{
    ESP_LOGI(TAG, "[stop_charging_get_handler]");
    // send cmd to stm32
    // get charged energy
    // check stm32 state
    const char* temp = "20.5";

    Transaction->end_datetime = get_datetime_now();
    ESP_LOGI(TAG, "Charging end datetime - %s", Transaction->end_datetime);

    Transaction->charged_energy = temp; // replace with real value
    ESP_LOGI(TAG, "Charged energy - %s", Transaction->charged_energy);

    StationState = READY_STATE;
    https_client_post_transaction();
    https_client_post_state();

    return httpd_resp_sendstr(req, temp);
}

static esp_err_t progress_get_handler(httpd_req_t *req)
{
    ESP_LOGI(TAG, "[progress_get_handler]");
    // get real value from stm32
    const char* temp = "10.2";
    return httpd_resp_sendstr(req, temp);
}

static esp_err_t plugged_in_get_handler(httpd_req_t *req)
{
    ESP_LOGI(TAG, "[plugged_in_get_handler]");
    // get real value from stm32
    const char* temp = "1"; // 0 - false, 1 - true
    return httpd_resp_sendstr(req, temp);
}

#if CONFIG_EXAMPLE_ENABLE_HTTPS_USER_CALLBACK
void https_server_user_callback(esp_https_server_user_cb_arg_t *user_cb)
{
    ESP_LOGI(TAG, "Session Created!");
    const mbedtls_x509_crt *cert;

    const size_t buf_size = 1024;
    char *buf = calloc(buf_size, sizeof(char));
    if (buf == NULL) {
        ESP_LOGE(TAG, "Out of memory - Callback execution failed!");
        return;
    }

    cert = mbedtls_ssl_get_peer_cert(&user_cb->tls->ssl);
    if (cert != NULL) {
        mbedtls_x509_crt_info((char *) buf, buf_size - 1, "      ", cert);
        ESP_LOGI(TAG, "Peer certificate info:\n%s", buf);
    } else {
        ESP_LOGW(TAG, "Could not obtain the peer certificate!");
    }

    free(buf);
}
#endif

static const httpd_uri_t login = {
    .uri       = LOGIN_URI"*",
    .method    = HTTP_POST,
    .handler   = login_post_handler
};

static const httpd_uri_t logout = {
    .uri       = "/logout",
    .method    = HTTP_POST,
    .handler   = logout_post_handler
};

static const httpd_uri_t root = {
    .uri       = "/",
    .method    = HTTP_GET,
    .handler   = root_get_handler
};

// static const httpd_uri_t favicon = {
//     .uri       = "/favicon.ico",
//     .method    = HTTP_GET,
//     .handler   = favicon_get_handler
// };

static const httpd_uri_t backend_url = {
    .uri       = BACKEND_URL_URI"*",
    .method    = HTTP_POST,
    .handler   = backend_url_post_handler
};

static const httpd_uri_t station_id = {
    .uri       = STATION_ID_URI"*",
    .method    = HTTP_POST,
    .handler   = station_id_post_handler
};

static const httpd_uri_t start_charging = {
    .uri       = START_CHARGING_URI"*",
    .method    = HTTP_POST,
    .handler   = start_charging_post_handler
};

static const httpd_uri_t stop_charging = {
    .uri       = ESP32_API"/stop_charging",
    .method    = HTTP_GET,
    .handler   = stop_charging_get_handler
};

static const httpd_uri_t progress = {
    .uri       = ESP32_API"/progress",
    .method    = HTTP_GET,
    .handler   = progress_get_handler
};

static const httpd_uri_t plugged_in = {
    .uri       = ESP32_API"/plugged_in",
    .method    = HTTP_GET,
    .handler   = plugged_in_get_handler
};

static void register_uri_handlers(httpd_handle_t server)
{
    ESP_LOGI(TAG, "Registering URI handlers");
    httpd_register_uri_handler(server, &login);
    httpd_register_uri_handler(server, &logout);
    httpd_register_uri_handler(server, &root);
    httpd_register_uri_handler(server, &backend_url);
    httpd_register_uri_handler(server, &station_id);
    // esp32 web api handlers:
    httpd_register_uri_handler(server, &start_charging);
    httpd_register_uri_handler(server, &stop_charging);
    httpd_register_uri_handler(server, &progress);
    httpd_register_uri_handler(server, &plugged_in);
}

httpd_handle_t start_https_server(void)
{
    ESP_LOGI(TAG, "Starting https server");
    
    httpd_handle_t server = NULL;
    httpd_ssl_config_t conf = HTTPD_SSL_CONFIG_DEFAULT();

    extern const unsigned char cacert_pem_start[] asm("_binary_cacert_pem_start");
    extern const unsigned char cacert_pem_end[]   asm("_binary_cacert_pem_end");
    conf.cacert_pem = cacert_pem_start;
    conf.cacert_len = cacert_pem_end - cacert_pem_start;

    extern const unsigned char prvtkey_pem_start[] asm("_binary_prvtkey_pem_start");
    extern const unsigned char prvtkey_pem_end[]   asm("_binary_prvtkey_pem_end");
    conf.prvtkey_pem = prvtkey_pem_start;
    conf.prvtkey_len = prvtkey_pem_end - prvtkey_pem_start;

    #if CONFIG_EXAMPLE_ENABLE_HTTPS_USER_CALLBACK
    conf.user_cb = https_server_user_callback;
    #endif

    conf.httpd.max_uri_handlers = 10;
    conf.httpd.uri_match_fn = httpd_uri_match_wildcard;
    conf.httpd.lru_purge_enable = true;

    esp_err_t ret = httpd_ssl_start(&server, &conf);
    if (ESP_OK != ret) {
        ESP_LOGI(TAG, "Error starting server!");
        return NULL;
    }

    register_uri_handlers(server);
    srand(time(NULL));
    return server;
}

void stop_https_server(httpd_handle_t server)
{
    httpd_ssl_stop(server);
}