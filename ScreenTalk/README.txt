0) SignalR server runs as ASP.NET Core Blazor app hosted by nginx on Ubuntu 24.04.1 LTS host ('Ubuntu host')
    a. install nginx on Ubuntu host
    b. install ASP.NET Core on Ubuntu host
        > wget https://dot.net/v1/dotnet-install.sh -O dotnet-install.sh
        > sudo chmod +x ./dotnet-install.sh
        > sudo ./dotnet-install.sh --channel 9.0
1) Origin Server certificate covering '*.screentalk.cloud' and 'screentalk.cloud',
        to install on Ubuntu host, is stored in biffkittz@gmail.com cloudflare account
        and should be installed on the Ubuntu host
            a. the origin cert is installed at /etc/ssl/screentalkcer.pem
            b. the key file is installed at /etc/ssl/screentalkkey.pem
            c. These certs are backed up to BitWarden as well
2) .\screentalk_site_nginx gets added to /etc/nginx/sites-available/screentalk
        on Ubuntu 24.04.1 LTS host
3) Need to create a deployment script
    a. delete all files in `/var/www/screentalk/html` on the origin server
    b. copy all files needed to run the site from this repo (e.g., in `static_pre_launc_landing`) to `/var/www/screentalk/html` on the origin server
    c. execute `sudo systemctl restart nginx` on the origin server to restart nginx and enable the updated site
    d. build a self-contained app for linux-x64
        > sudo dotnet publish -r linux-x64 --self-contained
        > install the outputted self-contained app to var/www/screentalk
        > sudo ln -s /etc/nginx/sites-available/screentalk /etc/nginx/sites-enabled/screentalk
        > sudo nano /etc/nginx/sites-available/screentalk with content in .\screentalk_nginx_reverse_proxy
        > sudo dotnet screentalk.dll
    e. monitor the app by creating a service file to keep it running
        > sudo nano /etc/systemd/system/kestrel-screentalk.service
        > /etc/systemd/system/kestrel-screentalk.service gets content from sourced .\kestrel-screentalk and saved
        > sudo systemctl enable kestrel-screentalk.service
        > sudo systemctl start kestrel-screentalk.service
        > sudo systemctl status kestrel-screentalk.service
    f. View logs from Kestrel
        > sudo journalctl -fu kestrel-screentalk.service
        > sudo journalctl -fu kestrel-screentalk.service --since "2025-03-25" --until "2025-03-27 23:00"
    g. install and config ufw
        > sudo apt-get install ufw
        > sudo ufw allow 22/tcp
        > sudo ufw allow 80/tcp
        > sudo ufw allow 443/tcp
        > sudo ufw enable
    h. Change the Nginx response name
        > sudo nano src/http/ngx_http_header_filter_module.c
        > 


PS C:\WINDOWS\system32> cd C:\compile\project322\ScreenTalk\ScreenTalkExtension\SignalRClient\ts
PS C:\compile\project322\ScreenTalk\ScreenTalkExtension\SignalRClient\ts> npm run release