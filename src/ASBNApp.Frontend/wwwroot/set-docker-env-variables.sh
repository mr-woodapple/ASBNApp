#!/bin/sh

# Set the Web Assembly appsettings.json via a script

local_var=$(printenv "ApiUrl")
echo "ApiUrl from docker env is: $local_var"

sed -i "s|replace_with_script|${local_var}|g" appsettings.json
echo "replaced value 'replace_with_script' in appsettings.json with ${local_var}"