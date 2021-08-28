<template>
  <v-container fill-height>
    <v-row justify="center" align-content="start" style="max-height: 90vh; height: 90vh">
      <v-col cols="12" md="6" style="max-height: 100%; height: 100%">
        <card-shortlinks-gernerator />
      </v-col>
      <v-col cols="12" md="6" style="max-height: 100%; height: 100%">
        <card-shortlinks-list />
      </v-col>
    </v-row>
  </v-container>
</template>

<script lang="ts">
import Vue from "vue";
import CardShortlinksGernerator from "./CardShortlinksGernerator.vue";
import CardShortlinksList from "./CardShortlinksList.vue";

export default Vue.extend({
  components: { CardShortlinksGernerator, CardShortlinksList },
});
</script>

<style scoped>
.truncate {
  overflow: hidden;
  text-overflow: ellipsis;
}
</style>
