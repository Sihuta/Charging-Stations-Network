#include "global.h"
#include <string.h>
#include <esp_log.h>
#include <esp_system.h>

char* Esp32ServerUrl = "https://{ip}/esp32/api";
const char* SystemBackendUrl = "http://192.168.31.244:45455/api";
const char* StationId = "1";

uint8_t StationState = READY_STATE;
struct transaction* Transaction = NULL;

static const char *TAG = "global_helper";

char* get_value_from_file(char* file_name, size_t max_length)
{
  ESP_LOGI(TAG, "Reading file %s...", file_name);
  FILE* f = fopen(file_name, "r");
  if (f == NULL) {
      ESP_LOGE(TAG, "Failed to open file for reading");
      return STRING_EMPTY;
  }

  char *value = (char*)malloc(sizeof(char) * max_length);
  fgets(value, max_length, f);
  fclose(f);
  ESP_LOGI(TAG, "Read value from file: '%s'", value);
  return value;
}

char* get_station_id()
{
  char* id_from_file = get_value_from_file(STATION_ID_FILE_NAME, STATION_ID_MAX_LENGTH);
  return strcmp(id_from_file, STRING_EMPTY) == 0 ?
    StationId : id_from_file;
}

char* get_system_backend_url()
{
  char* url_from_file = get_value_from_file(BACKEND_URL_FILE_NAME, BACKEND_URL_MAX_LENGTH);
  return strcmp(url_from_file, STRING_EMPTY) == 0 ?
    SystemBackendUrl : url_from_file;
}