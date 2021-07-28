<template>
  <v-navigation-drawer absolute temporary v-model="shouldShowData">
    <v-list nav dense>
      <v-list-item @click="toggleDarkTheme">
        <v-icon class="pr-2">mdi-theme-light-dark</v-icon> Dark/Light Theme
      </v-list-item>
    </v-list>
    <template v-slot:append>
      <v-btn block tile large color="error" dark>Delete Account</v-btn>
    </template>
  </v-navigation-drawer>
</template>

<script lang="ts">
import Vue from "vue";

export default Vue.extend({
  watch: {
    shouldShowData: function () {
      this.$emit("updateDrawer", this.shouldShowData);
    },
    shouldShow: function () {
      this.shouldShowData = this.shouldShow;
    },
  },
  data() {
    return {
      shouldShowData: false,
    };
  },
  props: {
    shouldShow: {
      type: Boolean,
      default: false,
    },
  },
  methods: {
    toggleDarkTheme(): void {
      this.$vuetify.theme.dark = !this.$vuetify.theme.dark;
      localStorage.darkTheme = this.$vuetify.theme.dark;
    },
  },
});
</script>

<style scoped></style>
