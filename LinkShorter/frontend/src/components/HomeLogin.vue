<template>
  <v-container fill-height justify-center>
    <v-card elevation="2">
      <v-card-title class="text-lg-h1 text-h4"> Login/Sign Up </v-card-title>
      <v-card-actions>
        <v-tabs v-model="shouldRegister" color="primary" slider-color="primary" fixed-tabs>
          <v-tab :key="false"> Login </v-tab>
          <v-tab :key="true"> Sign Up </v-tab>
        </v-tabs>
      </v-card-actions>
      <v-card-text>
        <form-login
          v-if="!shouldRegister"
          @success="
            (u) => {
              showSnackbar = true;
              snackbarColor = 'success';
              snackbarMessage = `${u}, you successfully logged in!`;
            }
          "
          @error-push="
            (e) => {
              showSnackbar = true;
              snackbarColor = 'error';
              snackbarMessage = e;
            }
          "
        />
        <form-register
          @error-push="
            (e) => {
              showSnackbar = true;
              snackbarColor = 'error';
              snackbarMessage = e;
            }
          "
          v-else-if="shouldRegister"
        />
      </v-card-text>
    </v-card>
    <v-snackbar :color="snackbarColor" v-model="showSnackbar">
      <v-btn icon @click.native="showSnackbar = false">
        <v-icon>mdi-close</v-icon>
      </v-btn>
      {{ snackbarMessage }}
    </v-snackbar>
  </v-container>
</template>

<script lang="ts">
import Vue from "vue";
import FormLogin from "./FormLogin.vue";
import FormRegister from "./FormRegister.vue";

export default Vue.extend({
  components: { FormLogin, FormRegister },
  name: "Home",
  data() {
    return {
      shouldRegister: false,
      snackbarMessage: "",
      showSnackbar: false,
      snackbarColor: "primary",
    };
  },
});
</script>

<style scoped></style>
