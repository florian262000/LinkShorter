<template>
  <v-navigation-drawer absolute temporary v-model="shouldShowData">
    <v-list nav dense>
      <v-list-item @click="toggleDarkTheme">
        <v-icon class="pr-2">mdi-theme-light-dark</v-icon> Dark/Light Theme
      </v-list-item>
    </v-list>
    <template v-if="$store.state.isLoggedIn" v-slot:append>
      <v-btn @click="logoutUser" block tile large color="error" dark>Logout</v-btn>
    </template>
  </v-navigation-drawer>
</template>

<script lang="ts">
import router from "@/router";
import axios from "axios";
import Vue from "vue";
import { mapMutations } from "vuex";

export default Vue.extend({
  watch: {
    shouldShowData: function () {
      this.$emit("updateDrawer", this.shouldShowData);
    },
    shouldShow: function () {
      this.shouldShowData = this.shouldShow;
    },
  },
  data() {
    return {
      shouldShowData: false,
    };
  },
  props: {
    shouldShow: {
      type: Boolean,
      default: false,
    },
  },
  methods: {
    ...mapMutations(["setIsLoggedIn", "setUsername", "setShortlinks"]),
    toggleDarkTheme(): void {
      this.$vuetify.theme.dark = !this.$vuetify.theme.dark;
      localStorage.darkTheme = this.$vuetify.theme.dark;
    },
    async logoutUser(): Promise<void> {
      console.log("test");
      try {
        const response = await axios.delete("api/login/logout", { withCredentials: true });

        if (response.status !== 200) {
          throw response;
        }

        this.shouldShowData = false;
        router.push("/");
        this.setIsLoggedIn(false);
        this.setUsername("");
        this.setShortlinks([]);
      } catch (e) {
        console.log(`error loggin user out - ${e}`);
      }
    },
  },
});
</script>

<style scoped></style>
