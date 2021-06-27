<template>
  <div>
    <img src="logo-5.png" alt="logo" style="max-width: 27em" />
    <v-card height="100%" width="100%">
      <v-card-text height="100%" width="100%">
        <v-text-field placeholder="url" v-model="currentTargetUrl"
          >asdsad</v-text-field
        >
        <v-text-field
          placeholder="shortpath"
          v-model="currentShortPath"
        ></v-text-field>
        <v-btn @click="GenerateUniqueShortPath"> generate shortpath</v-btn>
        <v-btn @click="RequestShortLinkCreation"> add </v-btn>
      </v-card-text>
    </v-card>
    <v-list class="d-flex flex-column" height="20em"> </v-list>
    <v-card height="100%" width="100%">
      <v-card-text height="100%" width="100%">
        <v-btn @click="logout">Logout</v-btn>
      </v-card-text>
    </v-card>
  </div>
</template>

<script>
import LoginComponent from "@/components/LoginComponent";

export default {
  data() {
    return {
      currentShortPath: "",
      currentTargetUrl: "",
      username: "",
    };
  },
  mounted() {},
  methods: {
    GenerateUniqueShortPath: function () {
      let vm = this;
      this.$http
        .request({
          method: "get",
          url: "https://localhost:5001/api/link/getuniqueshortpath",
        })
        .then(function (response) {
          vm.currentShortPath = response.data;
        })
        .catch(function (error) {
          console.log(error);
        });
    },
    RequestShortLinkCreation: function () {
      let vm = this;
      this.$http
        .request({
          method: "post",
          url: "https://localhost:5001/api/link/add",
          data: {
            targetUrl: vm.currentTargetUrl,
            ShortPath: vm.currentShortPath,
          },
        })
        .then(function (response) {
          vm.currentShortPath = response.data;
        })
        .catch(function (error) {
          console.log(error);
        });
    },
    logout: function () {
      let vm = this;
      vm.$cookies.remove("session");
      console.log("btn clicked");
      vm.$emit("updateComponent", LoginComponent);
    },
  },
};
</script>

<style>
</style>