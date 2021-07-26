<template>
  <v-container>
    <a target="_blank" :href="`/${userShortlink.ShortPath}`">
      <h1>{{ `${$domainName}/${userShortlink.ShortPath}` }}</h1>
    </a>
    <br />
    <h2>Links to: {{ userShortlink.TargetUrl }}</h2>
    <br />
    <h2>Used {{ userShortlink.ClickCounter }} times</h2>
    <br />
    <h2>Created at {{ userShortlink.TimeStamp }}</h2>
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

<style scoped>
/* unvisited link */
a:link,
a:visited {
  color: grey;
  text-decoration: none;
}

/* mouse over link */
a:hover {
  color: dodgerblue;
}
</style>
