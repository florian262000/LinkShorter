<template>
  <v-card elevation="2" height="100%">
    <v-card-title primary-title class="text-h3"> Shortlinks </v-card-title>
    <v-card-text v-if="$store.state.shortlinks.length">
      <v-list v-for="(userShortlink, index) in $store.state.shortlinks" :key="index">
        <container-link :userShortlink="userShortlink" />
      </v-list>
    </v-card-text>
  </v-card>
</template>

<script lang="ts">
import Vue from "vue";
import { mapActions } from "vuex";
import ContainerLink from "./ContainerLink.vue";

export default Vue.extend({
  // eslint-disable-next-line vue/no-unused-components
  components: { ContainerLink },
  mounted() {
    this.loadShortlinks();
  },
  methods: {
    ...mapActions(["fetchShortlinks"]),
    loadShortlinks(): void {
      try {
        this.fetchShortlinks();
      } catch (e) {
        if (e.status < 500) {
          if (e.status === 401) {
            console.log("Shortlinks loading - can't be loaded, session invalid");
          } else {
            console.log("Shortlinks loading - unexpected error");
          }
        } else {
          console.log("Shortlinks loading - server error");
        }
      }
    },
  },
});
</script>

<style scoped>
.v-card {
  display: flex !important;
  flex-direction: column;
}

.v-card__text {
  flex-grow: 1;
  overflow-y: auto;
  overflow-x: hidden !important;
}
</style>
