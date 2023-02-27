#pragma once

#include <esp_system.h>

#define ESP32_API "/esp32/api"
#define MOUNT_POINT "/spiffs"

#define BACKEND_URL_FILE_NAME MOUNT_POINT"/b_url.txt"
#define STATION_ID_FILE_NAME MOUNT_POINT"/st_id.txt"

#define READY_STATE 0
#define CHARGING_STATE 1
#define ERROR_STATE 2

struct transaction {
    char* charged_energy;
    char* start_datetime;
    char* end_datetime;
};

extern char* Esp32ServerUrl;
extern char* SystemBackendUrl;
extern char* StationId;

extern uint8_t StationState;
extern struct transaction* Transaction;