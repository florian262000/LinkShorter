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
import { mapMutations } from "vuex";
export default Vue.extend({
  name: "App",

  components: {
    Navbar,

    HomeLogin,
    HomeDefault,
  },
  mounted() {
    this.loadTheme();
    this.loadSession();
  },
  methods: {
    ...mapMutations(["setUsername", "setIsLoggedIn"]),
    loadTheme(): void {
      if (localStorage.darkTheme) {
        this.$vuetify.theme.dark = JSON.parse(localStorage.darkTheme);
      }
    },
    loadSession(): void {
      const session = this.$cookies.get("session");
      if (session) {
        // TODO: actually fetch username from session cookie
        this.setUsername("kekwfuntkioniertnochnichtlmao");
        this.setIsLoggedIn(true);
      }
    },
  },
  data: () => ({}),
});
</script>

<style>
html {
  overflow-y: auto;
}
</style>
