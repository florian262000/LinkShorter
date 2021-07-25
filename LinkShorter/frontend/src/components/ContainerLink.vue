<template>
  <v-container>
    <h1>{{ $route.query.page + userShortlink.targetUrl }}</h1>
    <br />
    <h2>Links to: {{ userShortlink.targetUrl }}</h2>
    <br />
    <h2>Used {{ userShortlink.clickCounter }} times</h2>
    <br />
    <h2>Created at {{ userShortlink.timeStamp }}</h2>
  </v-container>
</template>

<script lang="ts">
import Vue from "vue";
import UserShortlink from "@/interfaces/userShortlink";

export default Vue.extend({
  props: {
    userShortlink: {
      type: Object as () => UserShortlink,
      default: {},
    },
  },
});
</script>

<style scoped></style>
