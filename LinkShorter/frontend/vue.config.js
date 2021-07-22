module.exports = {
  transpileDependencies: ["vuetify"],
  devServer: {
    proxy: {
      "^/api": {
        target: "http://localhost:5001",
        ws: true,
        changeOrigin: true,
      },
    },
  },
};
