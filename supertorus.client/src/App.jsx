import { useState } from 'react'
import './Styles/App.css'


//const [myvar, setmyvar] = useState();
function App() {
    const [parameters, setParameters] = useState({
        paramA: 1,
        paramB: '',
        paramC: ''
    })

    const [torus, SetTorus] = useState({
        outerRadius: 3,
        innerRadius: 1,
        centerX: 1,
        centerY: 1,
        centerZ: 1
    })


    const handleSubmit = async (e) => {
        e.preventDefault();
        //response = await fetch("https://catfact.ninja/fact");
        //fetch('https://jsonplaceholder.typicode.com/todos/1')
        //    .then(response => response.json())
        //    .then(json => console.log(json))
        let response = await fetch('/Amin/Torus/GetString', {
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            }
        });
        //let data = response.json();
        console.log(response.json());
        SetTorus(response);



        setParameters({
            paramA: '',
            paramB: '',
            paramC: ''
        })

    }

    return (
        <div>

            <h1>
                {torus.outerRadius}
                <br></br>
            </h1>

            <form onSubmit={handleSubmit}>
                <label>
                    Parametr A:
                    <input
                        type="text"
                        value={parameters.paramA}
                        onChange={(e) => setParameters({ ...parameters, paramA: e.target.value })}
                    />
                </label>
                <br />
                <label>
                    Parametr B:
                    <input
                        type="text"
                        value={parameters.paramB}
                        onChange={(e) => setParameters({ ...parameters, paramB: e.target.value })}
                    />
                </label>
                <br />
                <label>
                    Parametr C:
                    <input
                        type="text"
                        value={parameters.paramC}
                        onChange={(e) => setParameters({ ...parameters, paramC: e.target.value })}
                    />
                </label>
                <br />
                <button type="submit">Submit</button>
            </form>
        </div>
    );
}






export default App
