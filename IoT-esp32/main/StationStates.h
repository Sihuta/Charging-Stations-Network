#pragma once

#include <esp_system.h>

static char* states[] = {"READY", "CHARGING", "ERROR"};

static inline char* station_state_to_str(uint8_t s)
{
    return states[s];
}