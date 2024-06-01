import { Canvas } from '@react-three/fiber'
import { OrbitControls } from '@react-three/drei'

const TorusView = ({ torusList, axes }) => {
    return (
        <div style={{ width: 800, height: 600, backgroundColor: "black" }}>
            <Canvas >
                <OrbitControls />
                <axesHelper args={[axes]} />
                <mesh>
                    <boxGeometry args={[axes, axes, axes,15,15,15]} />
                    <meshNormalMaterial wireframe />
                </mesh>
                {torusList.map((tor, index) =>
                    <mesh key={index} position={[tor.centerX, tor.centerY, tor.centerZ]} >

                        <sphereGeometry args={[tor.radius,16, 16]} />
                        <meshNormalMaterial wireframe color='green' />
                    </mesh>
                )}
            </Canvas>
        </div>
    );
}

export default TorusView;