<template>
  <v-btn color="error" block @click="deleteAccount()">DELETE ACCOUNT</v-btn>
</template>

<script lang="ts">
import axios from "axios";
import Vue from "vue";
import { mapMutations } from "vuex";

export default Vue.extend({
  name: "UserDelete",
  methods: {
    ...mapMutations(["setIsLoggedIn", "setUsername", "setShortlinks"]),
    async deleteAccount(): Promise<void> {
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
