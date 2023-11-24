# HisoBOT
Бот принимает отчет через API после чего рассылает этот отчет по группам и каналам в которые добавил его одмин. Также вторым аргументом API является название проета по которому надо будет сделать рассылку.

### Принцип работы бота.
| API | Что принимает? |
| --- | --- |
| Bot API | [From body] (Update update) |
| Hisobot API | [From body] (string info, string projectName) |
- К боту имеет доступ только админы. 
Админы задаются в User model.
Админ имеет доступ к добавлению или удалению бота в группы и каналы.
    - Возможность админа видеть подключенные каналы и группы.
    - Добавление новых проектов (Прикрепления Project Name к группам и чатам).
- Если пользователь не найден среди пользователей бота, то отправлять user id в ответ.
- Кнопки у user: 
мои проекты (chat id : project name),
добавить проект,
удалить проект

### v.0.1
- Чужой человек не может добавлять или управлять ботом. Чужой человек — это user которого нету в БД.
- Если чужой человек добавит бота в канал или группу, то бот выходит оттуда.
- Админ может добавить бота в группу или канал.
- При добавлении бота в канал или группу админу, который его добавил приходит уведомление что бот добавлен в группу или канал.
- Админ, который добавил бота в группу всплывает уведомление с информацией о канале или группе.
- Админ может добавить произвольное количество групп или каналов к боту.
- По АПИ приходит информация и название проекта по которому надо сделать рассылку.
- Проверка на уникальность chat id при добавлении в БД.


---
### How to deploy ci/cd

Download the binary for your system <br />
`sudo curl -L --output /usr/local/bin/gitlab-runner https://gitlab-runner-downloads.s3.amazonaws.com/latest/binaries/gitlab-runner-linux-amd64`

Give it permission to execute <br />
`sudo chmod +x /usr/local/bin/gitlab-runner`

Install GitLab Runner <br />
`sudo gitlab-runner install --user=root --working-directory=/{dir}/`

Start runner <br />
`sudo gitlab-runner start`

install git <br />
`sudo apt install git`

/etc/systemd/system/hisobot.service
```
[Unit]
Description=hisobot

[Service]
WorkingDirectory=/home/otabek/builds/zseWQ-xm/0/mentos_dev/HisoBOT/bin/Release/net7.0/publish/
ExecStart=/usr/bin/dotnet /home/otabek/builds/zseWQ-xm/0/mentos_dev/HisoBOT/bin/Release/net7.0/HisoBOT.dll
Restart=always
# Restart service after 10 seconds if the dotnet service crashes:
RestartSec=10
KillSignal=SIGINT
SyslogIdentifier=dotnet-hisobot
User=otabek
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=ASPNETCORE_URLS=http://0.0.0.0:7777
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=true

[Install]
WantedBy=multi-user.target
```

> 🛠 Код для запуска сервиса <br/>
sudo systemctl enable PROJECT_NAME.service <br/>
sudo systemctl start PROJECT_NAME.service <br/>
sudo systemctl restart PROJECT_NAME.service

<<<<<<< HEAD
=======

-> PostgreSQL 15
-> dotnet sdk 7
-> aspnet core runtime 7
-> nginx
```
dotnet tool install --global dotnet-ef
cat << \EOF >> ~/.bash_profile
# Add .NET Core SDK tools
export PATH="$PATH:/root/.dotnet/tools"
EOF
```
source ~/.bash_profile

>>>>>>> 8ceb436fd3edde07c25a6ca6d248822400fcdb8d
