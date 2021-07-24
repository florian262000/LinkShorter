<template>
  <v-container> </v-container>
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
