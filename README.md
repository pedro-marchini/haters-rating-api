# hater-rating-api

dotnet tool install --global dotnet-ef

export PATH="$PATH:$HOME/.dotnet/tools/"

dotnet-ef

run docker compose
docker compose --env-file .env up -d;
docker exec hatersrating_app_1 c:\migration\HatersRating.Migration.exe;
