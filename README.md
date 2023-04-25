# example-dotnet-deploy-ubuntu
github action 을 통한 ubuntu 서버 배포 예제
> aws lightsail 기준으로 만든 예제입니다.

# 서비스 동작 여부 확인
```
sudo systemctl list-units weather-api.service 
```

# 80 포트 권한 부여

dotnet 프로세스가 80포트에 바인딩할 수 있는 권한을 주려면 setcap 명령을 사용하여 CAP_NET_BIND_SERVICE 능력을 부여해야 합니다. 이렇게 하면 해당 능력이 부여된 사용자는 특정 포트에서 실행 중인 서비스나 프로그램을 실행할 때 root 권한 없이도 포트에 연결할 수 있습니다.

1. setcap 패키지를 설치합니다. Ubuntu를 사용하는 경우 다음 명령을 사용합니다.
```
sudo apt-get update
sudo apt-get install libcap2-bin
```

2. setcap 명령을 사용하여 CAP_NET_BIND_SERVICE 능력을 부여합니다.
```bash
sudo setcap 'CAP_NET_BIND_SERVICE=+ep' {dotnet이 설치된 경로}
```

> dotnet의 심볼릭 링크가 아닌 실제 설치된 경로여야 합니다. `readlink -f $(which dotnet)` 명령으로 확인할 수 있습니다.

3. dotnet 프로세스가 80포트에 바인딩할 수 있는지 확인합니다.
```bash
sudo setcap -v 'CAP_NET_BIND_SERVICE=+ep' {dotnet이 설치된 경로}
```

4. 확인 후 서비스를 다시 시작합니다.
```bash
sudo systemctl daemon-reload
sudo systemctl restart dotnet-weatherAPI.service
```
