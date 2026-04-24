#!/bin/sh

# Set the Web Assembly appsettings.json via a script

local_var=$(printenv "ApiUrl")
echo "ApiUrl from docker env is: $local_var"

sed -i "s|\"ApiUrl\": \"replace_with_script\"|\"ApiUrl\": \"${local_var}\"|g" appsettings.json
echo "replaced value 'replace_with_script' in appsettings.json with ${local_var}"

allow_registration=$(printenv "AllowRegistration")
if [ -n "$allow_registration" ]; then
	sed -i "s|\"AllowRegistration\": \"true\"|\"AllowRegistration\": ${allow_registration}|g" appsettings.json
	echo "replaced value 'AllowRegistration' in appsettings.json with ${allow_registration}"
fi