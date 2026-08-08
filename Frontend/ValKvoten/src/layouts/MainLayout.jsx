import Menu from '../components/menu';
import Dashboard from '../pages/Dashboard';

const MainLayout = () => {
  return (
    <div className="wrapper">
        <header>
          <Menu />
        </header>

        <main>
          <Dashboard />
        </main>
      </div>
  )
}

export default MainLayout