#!/bin/bash
#
# Script Name: deploy.sh
#
# Author: Arrowgene.Ddon Team
# Date : 22.04.2022
#
# Description: The following script deploys the ddon server to a fresh ubuntu installation.

server_git="https://github.com/sebastian-heinz/Arrowgene.DragonsDogmaOnline.git"

domain_name="ddon.arrowgene.net"

server_dir="/var/ddon/server"
webhook_dir="/var/ddon/webhook"

work_dir="$PWD"
tmp_dir="$work_dir/tmp"

if [ "$EUID" -ne 0 ]
  then echo "Please run as root"
  exit
fi

if ! which webhook > /dev/null 2>&1; then    
echo "Installing webhook"
    apt-get install -y webhook
fi

if ! which git > /dev/null 2>&1; then
    echo "Installing git"
    apt-get update
    apt-get install -y git
fi

if ! which dotnet > /dev/null 2>&1 || ! dotnet --list-sdks | grep -q '6.0.202'; then  
    echo "Installing dotnet 6.0.202"
    
    wget https://packages.microsoft.com/config/ubuntu/18.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
    dpkg -i packages-microsoft-prod.deb
    rm packages-microsoft-prod.deb
    
    apt-get install -y apt-transport-https
    apt-get update
    apt-get install -y dotnet-sdk-6.0.202
fi

systemctl stop ddon-server
systemctl stop webhook

## ensure directories
mkdir -p "$server_dir"
mkdir -p "$webhook_dir"

## copy files
#cp -R "$work_dir/nginx/." "$nginx_dir"
#cp -R "$work_dir/sendmail/." "$sendmail_dir"
#cp "$work_dir/opendkim/opendkim.conf" /etc/opendkim.conf
#cp "$work_dir/opendkim/opendkim" /etc/default/opendkim

## delete temp files
echo "Cleaning /tmp Files"
rm -rf "$tmp_dir"

## setup ddon server
echo "Installing Necromancy Server"

## delete all server files
rm -rf $server_dir/
mkdir -p "$server_dir"

tmp_server_dir="$tmp_dir/server"
git clone --single-branch -b feature/deploy "$server_git" "$tmp_server_dir"
dotnet publish "$tmp_server_dir/Arrowgene.Ddon.Cli/Arrowgene.Ddon.Cli.csproj" /p:Version=1 --runtime linux-x64 --configuration Release --output $tmp_server_dir/publish
cp -r "$tmp_server_dir/publish/." "$server_dir/."
cp "$tmp_server_dir/deploy/Arrowgene.Ddon.config.json" "$server_dir/Files/Arrowgene.Ddon.config.json"
cp "$tmp_server_dir/deploy/GameServerList.csv" "$server_dir/Files/GameServerList.csv"


echo "Creating Ddon Server Service"
adduser --disabled-password --gecos "" ddon_server
chown -R ddon_server:ddon_server "$server_dir"
rm /lib/systemd/system/ddon-server.service
cat << EOF >> /lib/systemd/system/ddon-server.service
[Unit]
After=network.target
[Service]
Type=simple
User=ddon_server
ExecStart=$server_dir/Arrowgene.Ddon.Cli server start --service
WorkingDirectory=$server_dir
Restart=on-failure
RestartSec=600
[Install]
WantedBy=multi-user.target
EOF


##setup webhook
cp "$work_dir/webhook/hooks.json" "$webhook_dir/hooks.json"

chown -R root:root "$webhook_dir"
rm /lib/systemd/system/webhook.service
cat << EOF >> /lib/systemd/system/webhook.service
[Unit]
After=network.target
[Service]
User=root
Group=root
ExecStart=/usr/bin/webhook -hooks $webhook_dir/hooks.json -verbose
WorkingDirectory=$webhook_dir
Restart=on-failure
RestartSec=600
[Install]
WantedBy=multi-user.target
EOF


## update services
echo "Enabling services"
systemctl daemon-reload

systemctl enable ddon-server
systemctl restart ddon-server

systemctl enable webhook
systemctl restart webhook

echo "Setup Completed"