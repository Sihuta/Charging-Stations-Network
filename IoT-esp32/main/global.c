#include "global.h"

char* Esp32ServerUrl = "https://{ip}/esp32/api";
char* SystemBackendUrl = "http://192.168.31.244:45455/api";
char* StationId = "0";

uint8_t StationState = READY_STATE;
struct transaction* Transaction = NULL;