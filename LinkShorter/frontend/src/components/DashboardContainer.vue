<template>
  <v-main>
    <v-row>
      <v-col cols="3">
        <leftDashboardPart @updateComponent="updateComponent" />
      </v-col>
      <v-col>
        <RightDashboardPart />
      </v-col>
    </v-row>
  </v-main>
</template>

<script>
import RightDashboardPart from "@/components/RightDashboardPart";
import LeftDashboardPart from "@/components/LeftDashboardPart";

export default {
  name: "MainComponent",
  components: {
    RightDashboardPart,
    LeftDashboardPart,
  },
  methods: {
    updateComponent: function (componentName) {
      this.$emit("updateComponent", componentName);
    },
  },
  data: () => ({}),
};
</script>
