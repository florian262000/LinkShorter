<template>
  <v-container fill-height>
    <v-row>
      <v-col>
        <h1 class="text-center text-md-h2 text-h4 truncate">
          Good to see you, <br />
          {{ $store.state.username }}
        </h1>
      </v-col>
    </v-row>
    <v-row justify="center" class="fill-height">
      <v-col cols="12" md="6" align-self="stretch">
        <card-shortlinks-gernerator />
      </v-col>
      <v-col cols="12" md="6">
        <card-userpanel />
      </v-col>
    </v-row>
  </v-container>
</template>

<script lang="ts">
import Vue from "vue";
import CardShortlinksGernerator from "./CardShortlinksGernerator.vue";
import CardUserpanel from "./CardShortlinksList.vue";

export default Vue.extend({
  // eslint-disable-next-line vue/no-unused-components
  components: { CardShortlinksGernerator, CardUserpanel },
});
</script>

<style scoped>
.truncate {
  overflow: hidden;
  text-overflow: ellipsis;
}
</style>
