#!/bin/sh

# Set the Web Assembly appsettings.json via a script

api_url=$(printenv "ApiUrl")
echo "ApiUrl from docker env is: $api_url"

sed -i "s|\"ApiUrl\": \"replace_with_script\"|\"ApiUrl\": \"${api_url}\"|g" appsettings.json
echo "Replaced value 'replace_with_script' in appsettings.json with ${api_url}"

allow_registration=$(printenv "FeatureFlags__AllowRegistration")
echo "AllowRegistration from docker env is: $allow_registration"

if [ -n "$allow_registration" ]; then
	sed -i "s|\"AllowRegistration\": \"true\"|\"AllowRegistration\": ${allow_registration}|g" appsettings.json
	echo "Replaced value 'AllowRegistration' in appsettings.json with ${allow_registration}"
fi