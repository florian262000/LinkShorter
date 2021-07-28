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
import axios from "axios";

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
    async loadSession(): Promise<void> {
      const session = this.$cookies.get("session");

      !(await this.invalidateSession()) && this.$cookies.remove("session");

      if (session) {
        try {
          const username = await this.fetchUsernameFromSession();

          this.setUsername(username);
          this.setIsLoggedIn(true);
        } catch (e) {
          if (e.status < 500) {
            if (e.status === 401) {
              console.log("Get username - unauthorized");
            } else {
              console.log("Get username - unexpected");
            }
          } else {
            console.log("Get username - server error");
          }
        }
      }
    },
    async fetchUsernameFromSession(): Promise<any> {
      const response = await axios.post("/api/login/getusername", {}, { withCredentials: true });

      if (response.status !== 200) {
        throw response;
      }

      const data = await response.data.name;
      return data;
    },
    async invalidateSession(): Promise<boolean> {
      try {
        const response = await axios.post(`api/login/validatesession/${this.$cookies.get("session")}`);

        if (response.status !== 200) {
          throw response;
        }

        return true;
      } catch (e) {
        // axios throws exception when a 404 is returned, so this has to be handled differently
        if (e.response.status === 404) {
          return false;
        }

        if (e.status < 500) {
          console.log("validate session - unexpected error");
        } else {
          console.log("validate session - server error");
        }

        return false;
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
