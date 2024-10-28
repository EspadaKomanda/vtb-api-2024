import Image from "next/image";
import "../styles/main.css";
import NavigationComponent from "../components/navigation_component";
import MainFeatures from "../components/main_features";

export default function Home() {
  return (
    <>
      <header className="main-bg background-section pt-8">
        <NavigationComponent/>
        <MainFeatures />
      </header>
    </>
  );
}
