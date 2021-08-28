<template>
  <home-login v-if="!$store.state.isLoggedIn" />
  <home-default v-else />
</template>

<script lang="ts">
import Vue from "vue";
import HomeLogin from "@/components/HomeLogin.vue";
import HomeDefault from "@/components/HomeDefault.vue";

export default Vue.extend({
  name: "Home",

  components: {
    HomeLogin,
    HomeDefault,
  },
});
</script>

<style>
html {
  overflow-y: auto;
}
</style>
