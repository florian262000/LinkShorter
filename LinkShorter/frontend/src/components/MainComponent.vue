<template>
  <v-main>
    <div :is="currentComponent" @updateComponent="updateComponent"></div>
  </v-main>
</template>

<script>
import LoginComponent from "@/components/LoginComponent";
import DashboardContainer from "./DashboardContainer.vue";

export default {
  name: "MainComponent",
  components: {
    DashboardContainer,
    LoginComponent,
  },
  methods: {
    updateComponent: function (componentName) {
      console.log("componentName" + componentName);
      let vm = this;
      vm.currentComponent = componentName;
    },
  },
  mounted() {
    let vm = this;
    let session = $cookies.get("session");
    if (session == null || session === "") {
      this.currentComponent = LoginComponent;
    } else {
      // verify cookie
      this.$http
        .request({
          method: "post",
          url: "https://localhost:5001/api/login/validatesession/" + session,
        })
        .then(function (response) {
          console.log(response);
          if (response.data === true) {
            console.log("should change component");
            vm.currentComponent = DashboardContainer;
          }
        })
        .catch(function (error) {
          console.log(error);
        });
    }
  },
  data: () => ({
    currentComponent: LoginComponent,
  }),
};
</script>
