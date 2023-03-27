#pragma once

#include "global.h"

#define STA_CONTROLLER "Stations"
// #define STATE_QUERY_PARAM "state"
#define CONN_INFO_ENDPOINT "connectionInfo"
#define TRANS_ENDPOINT "transaction"

#define CONN_INFO_URL_FIELD "serverUrl"
#define CONN_INFO_SSID_FIELD "localNetworkSsid"
#define CONN_INFO_PWD_FIELD "localNetworkPwd"

#define TRANS_ENERGY_FIELD "chargedEnergy"
#define TRANS_START_FIELD "startDatetime"
#define TRANS_END_FIELD "endDatetime"

void https_client_post_state();
void https_client_post_conn_info();
void https_client_post_transaction();