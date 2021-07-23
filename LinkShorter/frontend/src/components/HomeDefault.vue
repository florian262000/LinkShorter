<template>
  <v-container d-flex grid-list-xs fill-height>
    <v-col align-self="center">
      <h1 class="text-center text-h1">Good to see you, {{ $store.state.username }}</h1>
    </v-col>
  </v-container>
</template>

<script lang="ts">
import Vue from "vue";

export default Vue.extend({});
</script>

<style scoped></style>
