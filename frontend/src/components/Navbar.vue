<template>
  <v-app-bar app flat dark>
    <v-btn @click.stop="$emit('toggleDrawer')" icon>
      <v-icon>mdi-cog</v-icon>
    </v-btn>
    <v-spacer></v-spacer>
    <v-app-bar-title>
      <div @click="home" class="text-center text-h3" id="to-home">LinkShorter</div>
    </v-app-bar-title>
    <v-spacer></v-spacer>
    <v-btn href="https://github.com/florian262000/LinkShorter" target="_blank" icon>
      <v-icon>mdi-github</v-icon>
    </v-btn>
  </v-app-bar>
</template>

<script lang="ts">
import Vue from "vue";

export default Vue.extend({
  methods: {
    home() {
      if (this.$router.currentRoute.path !== "/") {
        this.$router.push("/");
      }
    },
  },
});
</script>

<style scoped>
div#to-home {
  cursor: pointer;
}
</style>
