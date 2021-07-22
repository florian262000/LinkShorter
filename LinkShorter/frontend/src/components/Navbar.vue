<template>
  <v-app-bar app flat dark>
    <v-btn @click="$store.state.isLoggedIn = !$store.state.isLoggedIn" color="error" icon>
      <v-icon>mdi-dev-to</v-icon>
    </v-btn>
    <v-btn @click="toggleDarkTheme()" icon>
      <v-icon>mdi-theme-light-dark</v-icon>
    </v-btn>
    <v-spacer></v-spacer>
    <v-app-bar-title> <div class="text-center text-h3">LinkShorter</div> </v-app-bar-title>
    <v-spacer></v-spacer>
    <v-btn href="https://github.com/florian262000/LinkShorter" target="_blank" icon>
      <v-icon>mdi-github</v-icon>
    </v-btn>
  </v-app-bar>
</template>

<script lang="ts">
import Vue from "vue";

export default Vue.extend({
  methods: {
    toggleDarkTheme(): void {
      this.$vuetify.theme.dark = !this.$vuetify.theme.dark;
      localStorage.darkTheme = this.$vuetify.theme.dark;
    },
  },
});
</script>

<style scoped></style>
