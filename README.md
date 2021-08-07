# LinkShorter

docker-copmpose:
```
version: "3.3"
services:
  linkshorter:
    container_name: linkshorter-app
    image: public.ecr.aws/v1e8u6g7/linkshorter:latest
    restart: always
    ports:
      - "80:80"
    environment:
      - database_host=linkshorter-database
      - database_username=linkshorter
      - database_password=Su9fha79YKnnHCaQrvHuQyqHk
      - database_name=linkshorter
      - urlbase=http://stelz.de
      - password_pepper=31jHVOEh4tgK2PeQ1R4z7n93lYOSQQvxOSOPbqPRHoOtriu17PX1K2zppMko
  db:
    container_name: linkshorter-database
    image: public.ecr.aws/ubuntu/postgres:latest
    restart: always
    environment:
      POSTGRES_DB: linkshorter
      POSTGRES_USER: linkshorter
      POSTGRES_PASSWORD: Su9fha79YKnnHCaQrvHuQyqHk
    volumes:
      - database-content:/var/lib/postgresql/data

volumes:
  database-content:
```
