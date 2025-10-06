# Food Ordering System - Deployment Guide

A microservices-based food ordering platform built with .NET Core 8, Blazor, and PostgreSQL, deployed on Kubernetes.

## Table of Contents
- [Prerequisites](#prerequisites)
- [Quick Start](#quick-start)
- [Detailed Setup Instructions](#detailed-setup-instructions)
- [Testing the Application](#testing-the-application)
- [Scaling](#scaling)
- [Troubleshooting](#troubleshooting)

---

### Microservices
- **User Service** (Port 8080): User authentication, JWT tokens, profile management
- **Restaurant Service** (Port 8081): Restaurant and menu management
- **Order Service** (Port 8082): Order processing with service-to-service communication
- **Frontend** (Port 5000/80): Blazor Server web application
- **PostgreSQL Database** (Port 5432): Persistent data storage

---

## Prerequisites

### Required Software
- **Docker Desktop** (with Kubernetes enabled)
- **.NET 8 SDK** (for local development/building)
- **kubectl** (Kubernetes CLI)
- **Git**
- **PowerShell** (Windows) or **Bash** (Linux/Mac)

### Enable Kubernetes in Docker Desktop
1. Open Docker Desktop
2. Go to Settings → Kubernetes
3. Check "Enable Kubernetes"
4. Click "Apply & Restart"
5. Wait 3-5 minutes for Kubernetes to start

Verify Kubernetes is running:
```bash
kubectl version
kubectl get nodes
```

Expected output:
```
NAME             STATUS   ROLES           AGE   VERSION
docker-desktop   Ready    control-plane   5m    v1.29.x
```

---

## Quick Start

### 1. Clone the Repository
```bash
git clone <your-repository-url>
cd food-ordering-system
```

### 2. Build Docker Images

Navigate to each service and build:

**User Service:**
```bash
cd /User-Services/
docker build -t user-services:latest .

```

**Restaurant Service:**
```bash
cd /Restaurant-Services/
docker build -t restaurant-service:latest .

```

**Order Service:**
```bash
cd /Order-Services
docker build -t order-services:latest .
cd ..
```

**Frontend:**
```bash
cd /FoodOrderApp
docker build -t food-frontend:latest .
cd ..
```

Verify all images are built:
```bash
docker images | grep -E "user-services|restaurant-service|order-services|food-frontend"
```

### 3. Deploy to Kubernetes

**Create namespace:**
```bash
kubectl create namespace food-ordering-system
```

**Deploy PostgreSQL:**
```bash
kubectl apply -f k8s/database/postgresql-deployment.yaml
```

Wait for PostgreSQL to be ready (this takes 30-60 seconds):
```bash
kubectl wait --for=condition=ready pod -l app=postgres -n food-ordering-system --timeout=300s
```

**Deploy Services:**
```bash
kubectl apply -f k8s/user-service/user-service-deployment.yaml
kubectl apply -f k8s/restaurant-service/restaurant-service-deployment.yaml
kubectl apply -f k8s/order-service/order-service-deployment.yaml
kubectl apply -f k8s/frontend/frontend-deployment.yaml
```

Wait for all deployments to be ready:
```bash
kubectl wait --for=condition=available deployment --all -n food-ordering-system --timeout=600s
```

### 4. Access the Application

Port-forward the frontend service:
```bash
kubectl port-forward -n food-ordering-system svc/frontend-service 80:80
```

Open your browser to: **http://localhost**

---

## Detailed Setup Instructions

### Project Structure
```
food-ordering-system/
├── src/
│   ├── UserService/
│   │   └── UserService.API/
│   ├── RestaurantService/
│   │   └── Restaurant_Services/
│   ├── OrderService/
│   │   └── Order_Services/
│   └── Frontend/
│       └── FoodOrderApp/
├── k8s/
│   ├── database/
│   │   └── postgresql-deployment.yaml
│   ├── user-service/
│   │   └── user-service-deployment.yaml
│   ├── restaurant-service/
│   │   └── restaurant-service-deployment.yaml
│   ├── order-service/
│   │   └── order-service-deployment.yaml
│   └── frontend/
│       └── frontend-deployment.yaml
└── README.md
```

### Kubernetes Resources Created

**Namespace:**
- `food-ordering` - Isolated environment for all resources

**Deployments:**
- `postgres` (1 replica)
- `user-service` (2 replicas, HPA: 2-10)
- `restaurant-service` (2 replicas, HPA: 2-10)
- `order-service` (2 replicas, HPA: 2-10)
- `frontend` (2 replicas)

**Services:**
- `postgres-service` (ClusterIP)
- `user-service` (ClusterIP)
- `restaurant-service` (ClusterIP)
- `order-service` (ClusterIP)
- `frontend-service` (LoadBalancer)

**Storage:**
- `postgres-pvc` (PersistentVolumeClaim: 5Gi)

**Configuration:**
- ConfigMaps for service URLs and database connections
- Secrets for JWT keys and database passwords

---

## Testing the Application

### Default Test Accounts

**Customer Account:**
- Email: `customer@test.com`
- Password: `Customer123!`

**Admin Account:**
- Email: `admin@foodordering.com`
- Password: `Admin123!`

### Complete User Journey

1. **Register/Login:**
   - Click "Register" to create a new account
   - Or login with test credentials

2. **Browse Restaurants:**
   - View 3 pre-seeded restaurants:
     - Pizza Palace (Italian)
     - Burger Barn (American)
     - Sushi Zen (Japanese)

3. **View Menu:**
   - Click "View Menu" on any restaurant
   - Browse menu items by category

4. **Add to Cart:**
   - Select quantity and click "Add to Cart"
   - Cart badge appears in navbar

5. **Place Order:**
   - Click cart icon
   - Review items
   - Click "Proceed to Checkout"
   - Enter delivery address
   - Click "Place Order"

6. **View Orders:**
   - Navigate to "My Orders"
   - View order history and status

### Verify Deployments

**Check all pods are running:**
```bash
kubectl get pods -n food-ordering
```

Expected output:
```
NAME                                  READY   STATUS    RESTARTS   AGE
postgres-xxxxx                        1/1     Running   0          5m
restaurant-service-xxxxx              1/1     Running   0          4m
restaurant-service-xxxxx              1/1     Running   0          4m
user-service-xxxxx                    1/1     Running   0          4m
user-service-xxxxx                    1/1     Running   0          4m
order-service-xxxxx                   1/1     Running   0          4m
order-service-xxxxx                   1/1     Running   0          4m
frontend-xxxxx                        1/1     Running   0          3m
frontend-xxxxx                        1/1     Running   0          3m
```

**Check services:**
```bash
kubectl get svc -n food-ordering
```

**View logs:**
```bash
# User Service logs
kubectl logs -n food-ordering -l app=user-service --tail=50

# Restaurant Service logs
kubectl logs -n food-ordering -l app=restaurant-service --tail=50

# Order Service logs
kubectl logs -n food-ordering -l app=order-service --tail=50

# Frontend logs
kubectl logs -n food-ordering -l app=frontend --tail=50
```

---

## Scaling

### Horizontal Pod Autoscaling (HPA)

All services are configured with HPA:

**View current HPA status:**
```bash
kubectl get hpa -n food-ordering
```

**Manual scaling:**
```bash
# Scale Restaurant Service to 5 replicas
kubectl scale deployment restaurant-service --replicas=5 -n food-ordering-system

# Scale Order Service to 3 replicas
kubectl scale deployment order-service --replicas=3 -n food-ordering-system
```

**Watch scaling in action:**
```bash
kubectl get pods -n food-ordering-system -w
```

### HPA Configuration

- **User Service**: CPU 70%, Memory 80%, Min: 2, Max: 10
- **Restaurant Service**: CPU 70%, Memory 80%, Min: 2, Max: 10
- **Order Service**: CPU 70%, Memory 80%, Min: 2, Max: 10

---

## Troubleshooting

### Common Issues

#### Pods Not Starting (ImagePullBackOff)

**Problem:** Kubernetes can't find the Docker images.

**Solution:**
```bash
# Verify images exist locally
docker images

# Check pod description for details
kubectl describe pod <pod-name> -n food-ordering

# Ensure imagePullPolicy is set to "Never" for local images
# in deployment YAML files
```

#### Database Connection Errors

**Problem:** Services can't connect to PostgreSQL.

**Solution:**
```bash
# Check if PostgreSQL is running
kubectl get pods -n food-ordering-system -l app=postgres

# Check PostgreSQL logs
kubectl logs -n food-ordering-system -l app=postgres

# Test database connection
kubectl exec -it -n food-ordering-system deployment/postgres -- psql -U postgres -l
```

#### Service Communication Failures

**Problem:** Order Service can't reach User/Restaurant Services.

**Solution:**
```bash
# Check if services are running
kubectl get svc -n food-ordering-system

# Check service endpoints
kubectl get endpoints -n food-ordering-system

# Test connectivity from Order Service
kubectl exec -it -n food-ordering-system deployment/order-service -- curl http://user-service:8080/swagger/index.html
```

#### Frontend Can't Access Backend

**Problem:** Frontend shows "No restaurants found" or login fails.

**Solution:**
```bash
# Check if all backend services are running
kubectl get pods -n food-ordering

# Verify ConfigMap settings
kubectl get configmap frontend-config -n food-ordering -o yaml

# Check frontend logs for errors
kubectl logs -n food-ordering -l app=frontend --tail=100
```

### Useful Commands

**Restart a deployment:**
```bash
kubectl rollout restart deployment <deployment-name> -n food-ordering
```

**Delete and recreate a deployment:**
```bash
kubectl delete deployment <deployment-name> -n food-ordering
kubectl apply -f k8s/<service>/deployment.yaml
```

**Access Swagger UI for services:**
```bash
# User Service
kubectl port-forward -n food-ordering svc/user-service 8080:8080
# Open: http://localhost:8080/swagger/index.html

# Restaurant Service
kubectl port-forward -n food-ordering svc/restaurant-service 8081:8081
# Open: http://localhost:8081/swagger/index.html

# Order Service
kubectl port-forward -n food-ordering svc/order-service 8082:8082
# Open: http://localhost:8082/swagger/index.html
```

**View resource usage:**
```bash
kubectl top pods -n food-ordering
kubectl top nodes
```

**Delete everything:**
```bash
kubectl delete namespace food-ordering
```

---

## API Endpoints

### User Service (Port 8080)
- `POST /api/auth/register` - Register new user
- `POST /api/auth/login` - User login
- `GET /api/users/profile` - Get current user profile
- `PUT /api/users/profile` - Update profile

### Restaurant Service (Port 8081)
- `GET /api/restaurants` - List all restaurants
- `GET /api/restaurants/{id}` - Get restaurant details
- `GET /api/restaurants/{id}/menu` - Get restaurant menu
- `POST /api/restaurants` - Create restaurant (admin)
- `POST /api/restaurants/{id}/menu/items` - Add menu item

### Order Service (Port 8082)
- `POST /api/orders` - Create new order
- `GET /api/orders` - Get user's orders
- `GET /api/orders/{id}` - Get order details
- `PUT /api/orders/{id}/status` - Update order status

---

## Configuration

### Environment Variables

Services use ConfigMaps and Secrets for configuration:

**Database Connection:**
```yaml
ConnectionStrings__DefaultConnection: "Host=postgres-service;Database=<DbName>;Username=postgres;Password=postgres"
```

**JWT Settings:**
```yaml
JwtSettings__SecretKey: <base64-encoded-secret>
JwtSettings__Issuer: "FoodOrderingSystem"
JwtSettings__Audience: "FoodOrderingSystem"
```

**Service URLs:**
```yaml
ServiceUrls__UserService: "http://user-service:8080"
ServiceUrls__RestaurantService: "http://restaurant-service:8081"
ServiceUrls__OrderService: "http://order-service:8082"
```


---

## Architecture Patterns Implemented

- **Microservices Architecture**: Domain-driven service boundaries
- **Database per Service**: Each service owns its data
- **API Gateway Pattern**: Frontend orchestrates multiple service calls
- **Service Discovery**: Kubernetes DNS for service-to-service communication
- **Circuit Breaker**: Graceful handling of service failures
- **Horizontal Scaling**: Independent scaling with HPA
- **Persistent Storage**: PersistentVolumeClaim for database
- **Health Checks**: Readiness and liveness probes
- **Configuration Management**: ConfigMaps and Secrets

---

## Security Features

- JWT-based authentication with ASP.NET Core Identity
- Role-based authorization (Customer, Admin, RestaurantOwner)
- Kubernetes Secrets for sensitive data
- Network isolation within Kubernetes namespace
- Non-root container users
- Input validation with Data Annotations
- HTTPS redirection

---

## Technologies Used

- **.NET Core 8**: Backend services
- **Blazor Server**: Frontend web application
- **PostgreSQL 15**: Database
- **Entity Framework Core**: ORM
- **Kubernetes**: Container orchestration
- **Docker**: Containerization
- **JWT**: Authentication
- **Swagger/OpenAPI**: API documentation
- **Blazored.LocalStorage**: Client-side storage

---

