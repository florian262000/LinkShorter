<template>
  <div>
    <v-card>
      <v-form>
        <v-container>
          <v-row>
            <v-col cols="12" sm="6">
              <v-text-field
                label="username"
                v-model="username"
                filled
              ></v-text-field>
            </v-col>
          </v-row>
          <v-row>
            <v-col cols="12" sm="6">
              <v-text-field
                label="password"
                v-model="password"
                filled
              ></v-text-field>
            </v-col>
          </v-row>
          <v-btn @click="startLogin">login</v-btn>
          <v-btn @click="startRegistration">register</v-btn>
        </v-container>
      </v-form>
    </v-card>
  </div>
</template>

<script>
import DashboardContainer from "@/components/DashboardContainer";
export default {
  name: "LoginComponent",
  data() {
    return { username: "", password: "" };
  },
  methods: {
    startLogin: function () {
      let vm = this;
      this.$http
        .request({
          method: "post",
          url: "https://localhost:5001/api/login/login",
          data: {
            Username: this.username,
            Password: this.password,
          },
        })
        .then(function (response) {
          console.log(response);
          vm.$emit("updateComponent", DashboardContainer);
        })
        .catch(function (error) {
          console.log(error);
        });
    },
    startRegistration: function () {
      let vm = this;
      this.$http
        .request({
          method: "post",
          url: "https://localhost:5001/api/login/register",
          data: {
            Username: this.username,
            Password: this.password,
          },
        })
        .then(function (response) {
          console.log(response);
          vm.$emit("updateComponent", DashboardContainer);
        })
        .catch(function (error) {
          console.log(error);
        });
    },
  },
};
</script>
