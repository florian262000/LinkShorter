<template>
  <v-app>
    <navbar />
    <v-main>
      <home-login v-if="!$store.state.isLoggedIn" />
      <home-default v-else />
    </v-main>
  </v-app>
</template>

<script lang="ts">
import Vue from "vue";
import HomeLogin from "./components/HomeLogin.vue";
import HomeDefault from "./components/HomeDefault.vue";
import Navbar from "./components/Navbar.vue";

export default Vue.extend({
  name: "App",

  components: {
    Navbar,
    HomeLogin,
    HomeDefault,
  },
  mounted() {
    if (localStorage.darkTheme) {
      this.$vuetify.theme.dark = JSON.parse(localStorage.darkTheme);
    }
  },
  data: () => ({}),
});
</script>

<style>
html {
  overflow-y: auto;
}
</style>
