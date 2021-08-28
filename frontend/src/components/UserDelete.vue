<template>
  <div>
    <v-btn color="error" block @click="deleteAccount()">DELETE ACCOUNT</v-btn>
    <br />
    <v-alert :value="alert" transition="scale-transition" color="warning" icon="mdi-alert-octagram">
      <p class="text-center">
        Are you <span class="font-weight-bold">sure</span> you want to delete your account? <br />
        If so, click the button above again, and you will be gone.
      </p>
    </v-alert>
  </div>
</template>

<script lang="ts">
import axios from "axios";
import Vue from "vue";
import { mapMutations } from "vuex";

export default Vue.extend({
  name: "UserDelete",
  data() {
    return {
      alert: false,
    };
  },
  methods: {
    ...mapMutations(["setIsLoggedIn", "setUsername", "setShortlinks"]),
    async deleteAccount(): Promise<void> {
      // Check if the alert box is shown, and only delete the account if it's not
      if (!this.alert) {
        this.alert = true;
        return;
      }

      try {
        const response = await axios.delete("/api/login/removeaccount", { withCredentials: true });

        if (response.status !== 200) {
          throw response;
        }

        this.setIsLoggedIn(false);
        this.setUsername("");
        this.setShortlinks([]);
        this.$router.push("/");
      } catch (e) {
        console.log(e);
      }
    },
  },
});
</script>

<style scoped></style>
