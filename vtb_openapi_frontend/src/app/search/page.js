import NavigationComponent from "@/components/navigation_component";
import SearchTourComponent from "@/components/serch_experience_component";
export default function Search() {
    return (
        <div>
            <header>
                <NavigationComponent/>
            </header>
            <main className="container p-5">
                <SearchTourComponent/>
            </main>
        </div>
    );
}