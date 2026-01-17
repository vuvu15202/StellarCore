import { useLocation } from "react-router-dom";
import Layout from "./layouts/Layout";
import WebLayout from "./layouts/WebLayout/Layout";
import AppRoutes from "./router/AppRoutes";
import { Blank } from "./layouts/Blank";

function App() {
  const location = useLocation();
  const isAuthPath = location.pathname.includes("auth") || location.pathname.includes("error") || location.pathname.includes("under-maintenance") | location.pathname.includes("blank");
  const isWebLayout = location.pathname.includes("web-layout");
  return (
    <>
      {isAuthPath ? (
        <AppRoutes>
          <Blank />
        </AppRoutes>
      ) : isWebLayout ? (
        <WebLayout>
          <AppRoutes />
        </WebLayout>
      ) : (
        <Layout>
          <AppRoutes />
        </Layout>
      )}
    </>
  );
}

export default App;
