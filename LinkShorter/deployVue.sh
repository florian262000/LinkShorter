cd frontend
npm run build
cd ..
if [ -d "wwwroot" ]; then
    mkdir wwwroot  
else 
  rm -r wwwroot/* 
fi
cp -r frontend/dist/*  wwwroot/
