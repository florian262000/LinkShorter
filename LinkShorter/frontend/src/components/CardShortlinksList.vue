<template>
  <v-card elevation="2" height="100%">
    <v-card-title primary-title class="text-h3"> Shortlinks </v-card-title>
    <v-card-text> https:// </v-card-text>
  </v-card>
</template>

<script lang="ts">
import Vue from "vue";
import { mapActions } from "vuex";

export default Vue.extend({
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

<style scoped></style>
