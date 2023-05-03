#pragma once

#include <esp_system.h>

#define ESP32_API "/esp32/api"
#define MOUNT_POINT "/spiffs"

#define BACKEND_URL_FILE_NAME MOUNT_POINT"/b_url.txt"
#define STATION_ID_FILE_NAME MOUNT_POINT"/st_id.txt"

#define BACKEND_URL_MAX_LENGTH 50
#define STATION_ID_MAX_LENGTH 4
#define ENERGY_MAX_LENGTH 5

#define STRING_EMPTY ""

#define READY_STATE 0
#define CHARGING_STATE 1
#define ERROR_STATE 2

struct transaction {
    char* charged_energy;
    char* start_datetime;
    char* end_datetime;
};

extern char* Esp32ServerUrl;

extern uint8_t StationState;
extern struct transaction* Transaction;

char* get_station_id();
char* get_system_backend_url();