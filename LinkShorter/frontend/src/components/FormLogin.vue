<template>
  <validation-observer ref="observer" v-slot="{ invalid, handleSubmit }">
    <form @submit.prevent="handleSubmit(submit)">
      <validation-provider v-slot="{ errors }" rules="required" name="username">
        <v-text-field type="text" label="Username" :error-messages="errors" v-model="username" required></v-text-field>
      </validation-provider>
      <validation-provider v-slot="{ errors }" rules="required" name="password">
        <v-text-field
          type="password"
          label="Password"
          :error-messages="errors"
          v-model="password"
          required
        ></v-text-field>
      </validation-provider>
      <v-btn text color="primary" type="submit" :disabled="invalid"> Login </v-btn>
    </form>
  </validation-observer>
</template>

<script lang="ts">
import Vue from "vue";
import { ValidationProvider, ValidationObserver } from "vee-validate";
import { mapMutations } from "vuex";

export default Vue.extend({
  components: {
    ValidationProvider,
    ValidationObserver,
  },
  data() {
    return {
      username: "",
      password: "",
      totp: null,
    };
  },
  methods: {
    ...mapMutations(["setIsLoggedIn"]),
    async submit(): Promise<void> {
      try {
        // const response = await fetch("/api/user/login", {
        //   method: "POST",
        //   headers: {
        //     "Content-Type": "application/json",
        //   },
        //   body: JSON.stringify({
        //     username: this.username,
        //     password: this.password,
        //     token: this.totp,
        //   }),
        // });

        // if (!response.ok) {
        //   throw response;
        // }

        // const data = await response.json();

        this.$emit("success");
        this.setIsLoggedIn(true);
        // this.setUserId(data.id);
        // this.setUsername(data.username);
      } catch (e) {
        const data = await e.json();
        this.$emit("error-push", data.errorMessage);
      }
    },
  },
});
</script>

<style></style>
