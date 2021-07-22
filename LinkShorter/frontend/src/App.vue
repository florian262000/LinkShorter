<template>
  <v-app>
    <navbar />
    <v-main>
      <home />
    </v-main>
  </v-app>
</template>

<script lang="ts">
import Vue from "vue";
import Home from "./components/Home.vue";
import Navbar from "./components/Navbar.vue";

export default Vue.extend({
  name: "App",

  components: {
    Navbar,
    Home,
  },

  data: () => ({}),
});
</script>

<style>
html {
  overflow-y: auto;
}
</style>
