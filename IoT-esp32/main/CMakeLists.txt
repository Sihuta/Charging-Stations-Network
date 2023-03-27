idf_component_register(SRCS "https_server.c" "https_client.c" "main.c" "global.c"
                    INCLUDE_DIRS "."
                    EMBED_FILES "upload_script.html" "upload_login_script.html" "favicon.ico"
                    EMBED_TXTFILES "certs/cacert.pem" "certs/prvtkey.pem" "certs/cert.pem")
