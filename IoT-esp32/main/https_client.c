#include "https_client.h"

#include <string.h>
#include "esp_system.h"
#include "esp_log.h"
#include "esp_http_client.h"
#include "esp_tls.h"

#include "global.h"
#include "StationStates.h"

static const char *TAG = "https_client";

void log_req_resp(esp_http_client_handle_t client)
{
    ESP_LOGI(TAG, "Status = %d, content_length = %d",
            esp_http_client_get_status_code(client),
            esp_http_client_get_content_length(client));
}

const char* get_req_url(const char* endpoint)
{
    char* url = NULL;
    char* backend_url = get_system_backend_url();
    char* sta_id = get_station_id();

    asprintf(&url, "%s/%s/%s/%s",
      backend_url,
      STA_CONTROLLER,
      sta_id,
      endpoint
    );

    free(backend_url);
    free(sta_id);
    ESP_LOGI(TAG, "req_url - %s", url);

    return (const char*) url;
}

const char* get_state_req_url(const char* state)
{
    char* url = NULL;
    char* backend_url = get_system_backend_url();
    char* sta_id = get_station_id();

    asprintf(&url, "%s/%s/%s/%s",
      backend_url,
      STA_CONTROLLER,
      sta_id,
      state
    );

    free(backend_url);
    free(sta_id);
    ESP_LOGI(TAG, "state_req_url - %s", url);

    return (const char*) url;
}

const char* get_conn_info_json()
{
    char* json = NULL;
    const char* format = "{\"%s\":\"%s\", \"%s\":\"%s\", \"%s\":\"%s\"}";
    asprintf(&json, format,
        CONN_INFO_URL_FIELD, Esp32ServerUrl,
        CONN_INFO_SSID_FIELD, CONFIG_EXAMPLE_WIFI_SSID,
        CONN_INFO_PWD_FIELD, CONFIG_EXAMPLE_WIFI_PASSWORD);

    ESP_LOGI(TAG, "conn_info_json - %s", json);
    return (const char*) json;
}

const char* get_transaction_json()
{
    char* json = NULL;
    const char* format = "{\"%s\":\"%s\", \"%s\":\"%s\", \"%s\":\"%s\"}";

    asprintf(&json, format,
        TRANS_ENERGY_FIELD, Transaction->charged_energy,
        TRANS_START_FIELD, Transaction->start_datetime,
        TRANS_END_FIELD, Transaction->end_datetime);

    ESP_LOGI(TAG, "trans_json - %s", json);
    return (const char*) json;
}

void https_client_post_state()
{
    ESP_LOGI(TAG, "[https_client_post_state]");
    
    const char* state = station_state_to_str(StationState);
    const char* req_url = get_state_req_url(state);
    esp_http_client_config_t config = {
        .url = req_url
    };

    esp_http_client_handle_t client = esp_http_client_init(&config);
    esp_http_client_set_method(client, HTTP_METHOD_POST);
    esp_err_t err = esp_http_client_perform(client);

    if (err == ESP_OK) {
        log_req_resp(client);
    }

    esp_http_client_cleanup(client);
}

void https_client_post_conn_info()
{
    ESP_LOGI(TAG, "[https_client_post_conn_info]");

    const char* req_url = get_req_url(CONN_INFO_ENDPOINT);
    const char* body = get_conn_info_json();
    esp_http_client_config_t config = {
        .url = req_url
    };

    esp_http_client_handle_t client = esp_http_client_init(&config);
    esp_http_client_set_method(client, HTTP_METHOD_POST);
    esp_http_client_set_header(client, "Content-Type", "application/json");
    esp_http_client_set_post_field(client, body, strlen(body));
    esp_err_t err = esp_http_client_perform(client);

    if (err == ESP_OK) {
        log_req_resp(client);
    }

    esp_http_client_cleanup(client);
}

void https_client_post_transaction()
{
    ESP_LOGI(TAG, "[https_client_post_transaction]");
    
    if (Transaction == NULL) {
        ESP_LOGW(TAG, "Transaction is null");
        return;
    }

    const char* req_url = get_req_url(TRANS_ENDPOINT);
    const char* body = get_transaction_json();

    esp_http_client_config_t config = {
        .url = req_url
    };

    esp_http_client_handle_t client = esp_http_client_init(&config);
    esp_http_client_set_method(client, HTTP_METHOD_POST);
    esp_http_client_set_header(client, "Content-Type", "application/json");
    esp_http_client_set_post_field(client, body, strlen(body));
    esp_err_t err = esp_http_client_perform(client);

    if (err == ESP_OK) {
        log_req_resp(client);
    }

    esp_http_client_cleanup(client);
}

// extern const char cert_pem_start[] asm("_binary_cert_pem_start");
// extern const char cert_pem_end[]   asm("_binary_cert_pem_end");
// .cert_pem = cert_pem_start,
// .cert_len = cert_pem_end - cert_pem_start