<template>
  <v-container>
    <v-row justify="start">
      <v-col align-self="top">
        <h1 class="text-center text-md-h2 text-h4 truncate">
          Good to see you, <br />
          {{ $store.state.username }}
        </h1>
      </v-col>
    </v-row>
    <v-row justify="center">
      <v-col cols="12" md="6">
        <card-shortlinks />
      </v-col>
      <v-col cols="12" md="6">
        <card-userpanel />
      </v-col>
    </v-row>
  </v-container>
</template>

<script lang="ts">
import Vue from "vue";
import CardShortlinks from "./CardShortlinksGernerator.vue";
import CardUserpanel from "./CardShortlinksList.vue";

export default Vue.extend({
  components: { CardShortlinks, CardUserpanel },
});
</script>

<style scoped>
.truncate {
  overflow: hidden;
  text-overflow: ellipsis;
}
</style>
