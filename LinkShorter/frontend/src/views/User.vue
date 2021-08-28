<template>
  <v-container fill-height>
    <v-row justify="center" align-content="start" style="max-height: 90vh; height: 90vh">
      <v-card elevation="2" height="100%" :loading="!$store.state.isLoggedIn">
        <v-card-title primary-title>
          <v-col>
            <h1 class="text-center text-md-h2 text-h4 truncate pa-2">
              Good to see you, <br />
              {{ $store.state.username }}
            </h1>
          </v-col>
        </v-card-title>
        <v-card-text>
          <user-api-key />
          <user-delete />
        </v-card-text>
      </v-card>
    </v-row>
  </v-container>
</template>

<script lang="ts">
import Vue from "vue";
import UserDelete from "@/components/UserDelete.vue";
import { mapGetters } from "vuex";
import UserApiKey from "@/components/UserApiKey.vue";

export default Vue.extend({
  components: {
    UserDelete,
    UserApiKey,
  },
  mounted() {
    if (!this.getIsLoggedIn()) {
      this.$router.push("/");
    }
  },
  methods: {
    ...mapGetters(["getIsLoggedIn"]),
  },
});
</script>

<style scoped></style>
