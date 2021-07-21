if [ ! -d "wwwroot" ]; then
    mkdir wwwroot  
else 
  rm -rf wwwroot/* 
fi

cd frontend
npm install 
npm run build
cd ..
cp -r frontend/dist/*  wwwroot/
